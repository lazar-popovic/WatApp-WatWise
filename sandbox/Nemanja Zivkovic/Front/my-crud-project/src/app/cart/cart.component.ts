import { Component } from '@angular/core';

@Component({
  selector: 'app-cart',
  template: `
    <button class="btn-add-to-cart" (click)="addToCart()">Add to Cart</button>
  `,
  styles: [
    `
      .btn-add-to-cart {
        background: linear-gradient(to bottom, #ff7f00, #ff5500);
        border: none;
        border-radius: 5px;
        color: #fff;
        cursor: pointer;
        font-size: 16px;
        padding: 10px 20px;
        text-align: center;
        text-decoration: none;
        text-shadow: 0 1px 1px rgba(0, 0, 0, 0.3);
      }

      .btn-add-to-cart:hover {
        background: linear-gradient(to bottom, #ff5500, #ff7f00);
      }
    `,
  ],
})
export class CartComponent {
  items: any[] = [];

  addToCart() {
    this.items.push({ name: 'Product', price: 9.99 });
    console.log('Item added to cart:', this.items);
  }
}
