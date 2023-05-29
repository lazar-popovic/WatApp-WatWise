import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from 'src/app/Models/User';
import { JWTService } from 'src/app/services/jwt.service';

@Component({
  selector: 'app-profile-card',
  templateUrl: './profile-card.component.html',
  styleUrls: ['./profile-card.component.css']
})
export class ProfileCardComponent implements OnInit {

  @Input() user = new User();
  @Output() showDelete = new EventEmitter<boolean>();
  @Output() showResend = new EventEmitter<boolean>();

  userId: any = this.jwtService.userId;
  roleId: any = this.jwtService.roleId;

  constructor( private jwtService: JWTService) { }

  ngOnInit() {
  }

}
