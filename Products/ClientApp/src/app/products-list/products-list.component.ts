import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { ProductsService } from '../products.service';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { ErrorService } from '../error.service';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent implements OnInit, OnDestroy {

  @Output() onAddProduct: EventEmitter<any> = new EventEmitter();
  @Output() onEditProduct: EventEmitter<any> = new EventEmitter();
  @Output() onDeleteProduct: EventEmitter<any> = new EventEmitter();

  columns: any[] = [];
  subscriptions: Subscription[] = [];
  showFriendlyMessage: boolean;
  productToDelete: any;

  constructor(private productsService: ProductsService, private errorService: ErrorService ) { }

  ngOnInit() {
    this.showFriendlyMessage = false;
    
    this.columns = ['id', 'name', 'description', 'quantity'];

    const sub = this.productsService.getProducts()
                .subscribe((products: any[]) => {
                  this.productsService.setProducts(products);

                  if (!this.hasProducts()) {
                    this.showFriendlyMessage = true;
                  }
                },
                (error) => {
                  this.errorService.handleError(error);
                });

    this.subscriptions.push(sub);
  }

  ngOnDestroy() {
    if (this.subscriptions.length > 0) {
      this.subscriptions.forEach(f => f.unsubscribe())
    }
  }

  add() {
    this.onAddProduct.emit();
  }

  edit(product: any) {
    this.onEditProduct.emit(product);
  }

  delete() {
    const sub = this.productsService.deleteProduct(this.productToDelete['id'])
                .pipe(
                  switchMap(() => {
                    return this.productsService.getProducts();
                  })
                )
                .subscribe((products: any[]) => {
                  this.productsService.setProducts(products);

                  if (!this.hasProducts()) {
                    this.showFriendlyMessage = true;
                  }
                },
                (error) => {
                  this.errorService.handleError(error);
                });

    this.subscriptions.push(sub);
  }

  setProductToDelete(product: any) {
    this.productToDelete = product;
  }

  hasProducts() {
    return this.productsService.products && this.productsService.products.length > 0;
  }
}
