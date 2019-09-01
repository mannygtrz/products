import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class ProductsService {
  private _products$: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  public products$: Observable<any[]> = this._products$.asObservable();
  get products(): any[] {
    return this._products$.getValue();
  }

  constructor(private http: HttpClient) { }

  editProduct(product: any) {
    return this.http.put('api/products', product);
  }

  addProduct(product: any) {
    return this.http.post('api/products', product);
  }

  deleteProduct(productId: number) {
    return this.http.delete(`api/products/${productId}`);
  }

  getProducts() {
    return this.http.get('api/products');
  }

  validateProduct(product: any) {
    return this.http.post('api/products/validate', product);
  }

  setProducts(products: any[]) {
    this._products$.next(products);
  }
}
