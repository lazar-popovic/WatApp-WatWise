using API.BL.Interfaces;
using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using API.Models.ViewModels;
using API.Services.E_mail.Interfaces;
using API.Services.JWTCreation.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Services.Geocoding.Interfaces;

namespace API.BL.Implementations
{
    public class AuthBL : IAuthBL
    {
        private readonly IAuthDAL _authDAL;
        private readonly IJWTCreator _jwtCreator;
        private readonly IMailService _mailService;
        private readonly ILocationDAL _locationDal;
        private readonly IGeocodingService _geocodingService;

        public AuthBL(IAuthDAL authDAL, IJWTCreator jwtCreator, IMailService mailService, ILocationDAL locationDal, IGeocodingService geocodingService)
        {
            _authDAL = authDAL;
            _jwtCreator = jwtCreator;
            _mailService = mailService;
            _locationDal = locationDal;
            _geocodingService = geocodingService;
        }

        public Response<LoginResponseViewModel> Login(LoginViewModel loginRequest)
        {
            var response = new Response<LoginResponseViewModel>();

            if (string.IsNullOrEmpty(loginRequest.Email.Trim()))
            {
                response.Errors.Add("Email is required");
                response.Success = false;
                return response;
            }

            var userWithRole = _authDAL.GetUserWithRoleForEmail(loginRequest.Email);

            response = ValidateUserWithRole(userWithRole);

            if (response.Success == false)
            {
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, userWithRole!.PasswordHash))
            {
                response.Errors.Add("Wrong password. Please check and try again");
                response.Success = false;
                return response;
            }
            response = GenerateToken(userWithRole!);
   
            return response;
        }

        public Response<RegisterResponseViewModel> RegisterUser(RegisterUserViewModel userRegisterRequest)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (string.IsNullOrEmpty(userRegisterRequest.Email.Trim()))
            {
                response.Errors.Add("Email is required");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Firstname.Trim()))
            {
                response.Errors.Add("Firstname is required");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Lastname.Trim()))
            {
                response.Errors.Add("Lastname is required");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Location.Address.Trim()))
            {
                response.Errors.Add("Address is required");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Location.City.Trim()))
            {
                response.Errors.Add("City is required");
            }
            
            if (userRegisterRequest.Location.Number == null)
            {
                response.Errors.Add("Number is required");
            }

            if (_authDAL.EmailExists(userRegisterRequest.Email))
                response.Errors.Add("User with this email already exists");

            response.Success = response.Errors.Count == 0;

            if (!response.Success)
            {
                return response;
            }
            ///// MAYBE MOVE TO LOCATION BL
            ///// BEGIN
            var cords = _geocodingService.Geocode(userRegisterRequest.Location);
            var locationId = _locationDal.GetLocationByLatLongAsync(cords);
            if (locationId == 0)
            {
                locationId = _locationDal.InsertLocation(userRegisterRequest.Location, cords);
            }
            ///// END
            var newUser = _authDAL.RegisterUser(userRegisterRequest, locationId);

            _mailService.sendToken(newUser);

            response.Data = new RegisterResponseViewModel { Message = "User registration successful!Check your email to complete registration." };

            return response;

        }

        public Response<RegisterResponseViewModel> RegisterEmployee(RegisterEmployeeViewModel employeeRegisterRequest)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (string.IsNullOrEmpty(employeeRegisterRequest.Email.Trim()))
            {
                response.Errors.Add("Email is required");
            }

            if (string.IsNullOrEmpty(employeeRegisterRequest.Firstname.Trim()))
            {
                response.Errors.Add("Firstname is required");
            }

            if (string.IsNullOrEmpty(employeeRegisterRequest.Lastname.Trim()))
            {
                response.Errors.Add("Lastname is required");
            }

            if (_authDAL.EmailExists(employeeRegisterRequest.Email))
                response.Errors.Add("Employee with this email already exists");

            response.Success = response.Errors.Count == 0;

            if (!response.Success)
            {
                return response;
            }

            User newEmployee = _authDAL.RegisterEmployee(employeeRegisterRequest);

            _mailService.sendToken(newEmployee);

            response.Data = new RegisterResponseViewModel { Message = "Employee registration successful!Check your email to complete registration." };

            return response;
        }

        public Response<RegisterResponseViewModel> ForgotPassword(ForgottenPasswordViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (string.IsNullOrEmpty(request.Email.Trim()))
            {
                response.Errors.Add("Email is required");
            }

            var userWithRole = _authDAL.GetUserWithRoleForEmail(request.Email);

            if (userWithRole == null)
            {
                response.Errors.Add("User with this email does not exists");
            }

            if (userWithRole?.Verified == false)
            {
                response.Errors.Add("This user is not verified. Please check your e-mail.");
            }

            response.Success = response.Errors.Count == 0;
            if (!response.Success)
            {
                return response;
            }

            var resetToken = GenerateNewResetPasswordToken(userWithRole!.Id);

            var resetJwtToken = GenerateJWTResetToken(userWithRole, resetToken);

            if (string.IsNullOrEmpty(resetJwtToken))
            {
                response.Errors.Add("Reset jwt token is not valid!");
                response.Success = response.Errors.Count == 0;

                return response;
            }  
            else
            {
                _mailService.sendResetToken(userWithRole, resetJwtToken);

                response.Data = new RegisterResponseViewModel { Message = "Verification mail has been sent to your email adress!" };
                return response;
            }
            
        }

        public Response<RegisterResponseViewModel> ResetPassword(ResetPasswordViewModel request)
        {

            var response = ValidatePasswordsAndResetToken(request);

            return response;
        }

        public Response<LoginResponseViewModel> RefreshToken(LoginResponseViewModel request)
        {
            var response = new Response<LoginResponseViewModel>();
            ClaimsPrincipal claimsPrincipal;
            //User user;
            string emailClaim;

            if(string.IsNullOrEmpty(request.Token.Trim()))
            {
                response.Errors.Add("Please provide access token!");
            }

            if (string.IsNullOrEmpty(request.RefreshToken.Trim()))
            {
                response.Errors.Add("Please provide refresh token!");
            }

            try
            {
                claimsPrincipal = _jwtCreator.GetPrincipalFromExpiredToken(request.Token);
                emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            }
            catch (SecurityTokenValidationException invalidTokenExc)
            {
                response.Errors.Add(invalidTokenExc.Message);
                response.Success = false;

                return response;
            }
            if (emailClaim == null)
            {
                response.Errors.Add("Error with expired access token!");
                response.Success = false;

                return response;
            }
               
            var user = _authDAL.GetUserWithRoleForEmail(emailClaim);

            if (user == null)
            {
                response.Errors.Add("User for given credentials doesen't exist!");
                response.Success = false;

                return response;
            }

            var refreshTokenFromBase = _authDAL.GetRefreshToken(user.Id);

            if (refreshTokenFromBase == null ||  refreshTokenFromBase.Token != request.RefreshToken || refreshTokenFromBase.Expires <= DateTime.Now || (refreshTokenFromBase.IsActive == false))
            {
                response.Errors.Add("Token is invalid!");
                response.Success = false;

                return response;
            }

            var newAccessToken = _jwtCreator.CreateToken(user);
            var newRefreshToken = _jwtCreator.GenerateRefreshToken();

             _authDAL.DeactivatePreviousRefreshTokensAndSaveNewToBase(user.Id, newRefreshToken);
            

            response.Data = new LoginResponseViewModel
            { 
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };

            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public Response<RegisterResponseViewModel> VerifyToken(VerifyTokenViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();
            int userId = 0;

            if (string.IsNullOrEmpty(request.Token))
            {
                response.Errors.Add("Token is empty!");
                response.Success = false;

                return response;
            }

            try
            {
                userId = _jwtCreator.GetUserIdFromToken(request.Token);
            }
            catch(SecurityTokenException se)
            {
                response.Errors.Add("Invalid token!" + se.InnerException?.Message);
            }
            catch(Exception e)
            {
                response.Errors.Add("An error occurred while verifying the email");
            }
            
            if(userId == -1) 
            {
                response.Errors.Add("Can not find user id!");
                response.Success = false;

                return response;
            }

            var user = _authDAL.FindUserById(userId);

            if(user == null)
            {
                response.Errors.Add("User doesen't exist!");
                response.Success = false;

                return response;
            }

            response.Data = new RegisterResponseViewModel { Message = user.Email! };
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public Response<RegisterResponseViewModel> VerifyAccount(VerifyAccountViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();
            int userId = 0;

            userId = _jwtCreator.GetUserIdFromToken(request.Token);

            if (string.IsNullOrEmpty(request.Password1.Trim()))
            {
                response.Errors.Add("Password is required");
            }
            if (string.IsNullOrEmpty(request.Password2.Trim()))
            {
                response.Errors.Add("Repeat password is required");
            }
            if (request.Password1 != request.Password2)
            {
                response.Errors.Add("Passwords need to match");
            }
            if (userId == -1)
            {
                response.Errors.Add("Invalid token!");
            }

            response.Success = response.Errors.Count == 0;

            if (!response.Success)
            {
                return response;
            }

            var user = _authDAL.FindUserById(userId);

            if(user == null)
            {
                response.Errors.Add("User doesen't exist!");
                response.Success = false;

                return response;
            }

            user!.Verified = true;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password1);

            //name may not be appropriate,but its just updating user nevermind the changes
            _authDAL.UpdateUserAfterPasswordReset(user);

            response.Data = new RegisterResponseViewModel
            {
                Message = "Verification successful!"
            };

            response.Success = response.Errors.Count() == 0; 
            return response;
        }

        public Response<RegisterResponseViewModel> ResendVerifyEmail(ResendVerifyEmailViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (string.IsNullOrEmpty(request.Email.Trim()))
            {
                response.Errors.Add("Email is required");
            }

            var user = _authDAL.GetUserWithRoleForEmail(request.Email);

            if (user == null)
            {
                response.Errors.Add("User doesen't exist!");
            }

            response.Success = response.Errors.Count() == 0;

            if (response.Success == false)
                return response;
            else
            {

                _mailService.resendToken(user!);

                response.Data = new RegisterResponseViewModel { Message = "New verfication mail has been sent!Check your email to complete registration." };

                return response;
            }
        }

        #region private



        private Response<LoginResponseViewModel> ValidateUserWithRole(User? userWithRole)
        {
            var response = new Response<LoginResponseViewModel>();

            if (userWithRole == null)
            {
                response.Errors.Add("User with this email does not exists");
            }
          
            if (userWithRole?.Verified == false)
            {
                response.Errors.Add("This user is not verified. Please check your e-mail.");
            }


            response.Success = response.Errors.Count == 0;
            return response;
        }

        private Response<RegisterResponseViewModel> ValidatePasswordsAndResetToken(ResetPasswordViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();
            JwtSecurityToken token; 

            if (string.IsNullOrEmpty(request.NewPassword))
            {
                response.Errors.Add("Password is empty");
            } 
            if (string.IsNullOrEmpty(request.ConfirmedNewPassword))
            {
                response.Errors.Add("Please repeat password");
            }
            if (request.ConfirmedNewPassword != request.NewPassword)
            {
                response.Errors.Add("Passwords need to match");
            }
            try
            {
                var handler = new JwtSecurityTokenHandler();
                token = handler.ReadJwtToken(request.Token);
            }
            catch(Exception ex) 
            {
                response.Errors.Add("Error while validating token!");
                response.Success = false;

                return response;
            }

            if (token == null)
            {
                response.Errors.Add("Error with reset token!");
                response.Success = response.Errors.Count() == 0;

                return response;
            }

            var resetToken = token!.Claims.FirstOrDefault(c => c.Type == "resetToken")?.Value;

            if (string.IsNullOrEmpty(resetToken))
            {
                response.Errors.Add("Token is null or empty!");
                response.Success = false;

                return response;
            }


            var tokenEntity = _authDAL.GetResetTokenEntity(resetToken);

            if (tokenEntity == null)
            {
                response.Errors.Add("Token is null or empty!");
                response.Success = false;

                return response;
            }

            if (DateTime.UtcNow > tokenEntity.ExpiryTime)
            {
                response.Errors.Add("Token expired");
                response.Success = false;

                return response;
            }

            var user = _authDAL.FindUserById((int)tokenEntity.UserId!);

            if (user == null)
            {
                response.Errors.Add("User not found!");
                response.Success = false;

                return response;
            }

            if (user.Verified == false)
            {
                response.Errors.Add("User not verified!");
                response.Success = false;

                return response;
            }

            SetNewPasswordAfterResetting(user, request.NewPassword);
            RemovePasswordResetTokenFromBase(tokenEntity);

            response.Success = response.Errors.Count() == 0;
            response.Data = new RegisterResponseViewModel { Message = "Password has been changed successfully!" };

            return response;
        }

        private void RemovePasswordResetTokenFromBase(ResetPasswordToken token)
        {
            _authDAL.RemoveResetToken(token);
        }

        private void SetNewPasswordAfterResetting(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _authDAL.UpdateUserAfterPasswordReset(user);
        }

        private Response<LoginResponseViewModel> GenerateToken(User userWithRole)
        {
            var response = new Response<LoginResponseViewModel>();
            var token = _jwtCreator.CreateToken(userWithRole!);
            var refreshToken = _jwtCreator.GenerateRefreshToken();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                response.Errors.Add("Something went wrong. Please try again later.");
            }
            else
            {
                try
                {
                    _authDAL.DeactivatePreviousRefreshTokensAndSaveNewToBase(userWithRole.Id, refreshToken);
                }
                catch(Exception ex)
                {
                    response.Errors.Add(token + " " + ex.Message);
                    response.Success = false;

                    return response;
                }

                var responseToken = new LoginResponseViewModel { RefreshToken = refreshToken, Token = token };
                response.Data = responseToken;
            }

            response.Success = response.Errors.Count == 0;
            return response;
        }

        private string GenerateJWTResetToken(User user, ResetPasswordToken resetToken)
        {
            var token = _jwtCreator.CreateResetToken(user.Id, user.Email!, resetToken);

            if (string.IsNullOrEmpty(token))
            {
               return null;
            }
            else
            {
                return token;
            }
        }

        private ResetPasswordToken GenerateNewResetPasswordToken(int userID)
        {
            var token = new ResetPasswordToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiryTime = DateTime.UtcNow.AddMinutes(15),
                UserId = userID
            };

            _authDAL.AddResetToken(token);

            return token;
        }

        #endregion
    }
}
