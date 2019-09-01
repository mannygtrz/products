import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { ProductsService } from '../products.service';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs/observable/of';
import { Subscription } from 'rxjs';
import { ErrorService } from '../error.service';

@Component({
  selector: 'app-products-details',
  templateUrl: './products-details.component.html',
  styleUrls: ['./products-details.component.css']
})
export class ProductsDetailsComponent implements OnInit, OnDestroy {
  @Input() editType: string;
  @Input() product: any;
  @Output() onExitForm: EventEmitter<any> = new EventEmitter();

  subscriptions: Subscription[] = [];
  productForm: FormGroup;
  isProductValid: boolean; 
  
  constructor(private fb: FormBuilder, private productsService: ProductsService, private errorService: ErrorService) { }

  ngOnInit() {
    this.isProductValid = true; 

    this.productForm = this.fb.group(
      {
        id: new FormControl(this.product ? this.product['id'] : 0),
        name: new FormControl(this.product ? this.product['name'] : null, {
          validators: Validators.required
       }),
        description: new FormControl(this.product ? this.product['description'] : null),
        quantity: new FormControl(this.product ? this.product['quantity'] : 0, {
          validators: [Validators.required, Validators.pattern('[+]?[0-9][0-9]*')]
       })
      }
    );
  }

  ngOnDestroy() {
    if (this.subscriptions.length > 0) {
      this.subscriptions.forEach(f => f.unsubscribe());
    }
  }

  submitForm() {
    this.isProductValid = true;

    const product = {
      id: this.productForm.get('id').value,
      name: this.productForm.get('name').value.trim(),
      description: this.productForm.get('description').value.trim(),
      quantity: this.productForm.get('quantity').value
    };

    switch(this.editType) { 
      case 'Add': { 
        this.confirmAddProduct(product);
        break; 
      } 
      case 'Edit': { 
        this.confirmEditProduct(product); 
        break; 
      }
    }
  }

  confirmAddProduct(product: any) {
    const sub = this.productsService.validateProduct(product)
                .pipe(
                  switchMap((isValidProduct: boolean) => {
                    if (!isValidProduct) {
                      this.isProductValid = false;
                      return of(undefined);
                    }

                    return this.productsService.addProduct(product);
                  }),
                  switchMap((results) => {
                    if (results === undefined) {
                      return of(undefined);
                    }
                    
                    return this.productsService.getProducts();
                  })
                )
                .subscribe((products: any[]) => {
                  if (products) {
                    this.productsService.setProducts(products);
                    this.exitForm();
                  }
                },
                (error) => {
                  this.errorService.handleError(error);
                });

    this.subscriptions.push(sub);
  }

  confirmEditProduct(product: any) {
    const sub = this.productsService.validateProduct(product)
                .pipe(
                  switchMap((isValidProduct: boolean) => {
                    if (!isValidProduct) {
                      this.isProductValid = false;
                      return of(undefined);
                    }

                    return this.productsService.editProduct(product);
                  }),
                  switchMap((results) => {
                    if (results === undefined) {
                      return of(undefined);
                    }
                    
                    return this.productsService.getProducts();
                  })
                )
                .subscribe((products: any[]) => {
                  if (products) {
                    this.productsService.setProducts(products);
                    this.exitForm();
                  }
                },
                (error) => {
                  this.errorService.handleError(error);
                });

    this.subscriptions.push(sub);
  }

  exitForm() {
    this.onExitForm.emit();
  }
}
