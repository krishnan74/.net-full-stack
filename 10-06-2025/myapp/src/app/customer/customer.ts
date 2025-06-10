import { Component } from '@angular/core';
import {customerData} from "./data.json";
import { CommonModule } from '@angular/common';

type CustomerType = {
  id: number;
  name: string;
  email?: string;
  phone?: string;
  address?: string;
  likes?: number;
  dislikes?: number;
  imageUrl?: string;
};

@Component({
  selector: 'app-customer',
  imports: [CommonModule],
  templateUrl: './customer.html',
  styleUrl: './customer.css'
})

export class Customer {
  customers: CustomerType[];
  name:string;

  constructor(){
    this.customers = customerData;
    this.name = customerData[0].name;
  }

  likeCustomer(customer: CustomerType) {
    const foundCustomer = this.customers.find(c => c.id === customer.id);
    if (foundCustomer) {
      foundCustomer.likes = (foundCustomer.likes || 0) + 1;
    }
  }

  dislikeCustomer(customer: CustomerType) {
    const foundCustomer = this.customers.find(c => c.id === customer.id);
    if (foundCustomer) {
      foundCustomer.dislikes = (foundCustomer.dislikes || 0) + 1;
    }
  }

}

