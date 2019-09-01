import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  editMode: boolean = false;
  editType: string;
  selectedProduct: any;

  constructor() { }

  ngOnInit() {
  }

  handleAddProduct() {
    this.editType = 'Add';
    this.selectedProduct = null;
    this.editMode = true;
  }
  handleEditProduct(product: any) {
    this.editType = 'Edit';
    this.selectedProduct = product;
    this.editMode = true;
  }

  handleExitForm() {
    this.editMode = false;
  }
}
