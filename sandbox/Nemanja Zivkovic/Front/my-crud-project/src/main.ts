import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));


const loginButton: HTMLElement | null = document.querySelector('.login-button');
const overlay: HTMLElement | null = document.querySelector('.overlay');
const loginForm: HTMLFormElement | null = document.querySelector('.login-form');

if (loginButton && overlay && loginForm) {
  loginButton.addEventListener('click', () => {
    overlay.style.display = 'block';
    loginForm.style.display = 'block';
  });

  overlay.addEventListener('click', () => {
    overlay.style.display = 'none';
    loginForm.style.display = 'none';
  });
}

interface User {
  id: number;
  name: string;
  email: string;
}

class CrudApp {
  private userList: User[];

  constructor() {
    this.userList = [];
  }

  createUser(name: string, email: string): void {
    const id = this.userList.length + 1;
    const user = { id, name, email };
    this.userList.push(user);
  }

  readUser(id: number): User | undefined {
    return this.userList.find(user => user.id === id);
  }

  readAllUsers(): User[] {
    return this.userList;
  }

  updateUser(id: number, name: string, email: string): void {
    const user = this.userList.find(user => user.id === id);
    if (user) {
      user.name = name;
      user.email = email;
    }
  }

  deleteUser(id: number): void {
    const index = this.userList.findIndex(user => user.id === id);
    if (index !== -1) {
      this.userList.splice(index, 1);
    }
  }
}

