import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot,Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth-service.service';
import { JWTService } from '../services/jwt.service';

@Injectable({
  providedIn: 'root'
})
export class OnlyUserGuardGuard implements CanActivate {
  constructor(private authService: AuthService,
              private jwtService: JWTService,
              private router: Router) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if(this.authService.isLogged == false || this.jwtService.roleId != 3) {
        this.router.navigateByUrl('login');
        return false;
      }
      return true;
  }

}
