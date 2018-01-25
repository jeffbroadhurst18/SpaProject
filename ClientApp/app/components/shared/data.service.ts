﻿import { Http, Response, Headers } from "@angular/http";
import { Injectable, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "./product";
import { Order, OrderItem } from "./order";
import 'rxjs/add/operator/map';

@Injectable()
export class DataService implements OnInit {
	ngOnInit(): void {
		this.loginRequired = true;
	}

	constructor(private http: Http) { }

	private token: string = "";

	private tokenExpiration: Date;

	public order: Order = new Order();

	public products: Product[] = [];

	loadProducts(): Observable<Product[]> {
		return this.http.get("/api/products")
			.map((result: Response) =>
				this.products = result.json());
	}

	AddToOrder(product: Product) {
		let item = this.order.items.find(i => i.productId == product.id);

		if (item) {
			item.quantity++;
		}
		else {
			item = new OrderItem();
			item.productCategory = product.category;
			item.productId = product.id;
			item.unitPrice = product.price;
			item.productSize = product.size;
			item.productTitle = product.title;
			item.productFilePath = product.filePath;
			item.quantity = 1;

			this.order.items.push(item);
		}
	}

	public get loginRequired(): boolean {
		return this.token.length == 0 || this.tokenExpiration < new Date();
	} // Either token doesn't exist or has expired.

	public set loginRequired(data) {
		if (data == true) {
			this.token = "";
		}
	}

	public login(creds:any) {
		return this.http.post("/account/createtoken", creds)
			.map(response => {
				let tokenInfo = response.json();
				this.token = tokenInfo.token;
				this.tokenExpiration = tokenInfo.expiration;
				return true;
			})
	}

	public checkout() {
		if (!this.order.orderNumber) {
			this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
		}
		return this.http.post("/api/orders", this.order, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		})
			.map(response => {
				this.order = new Order(); //Clears the current Order object
				return true;
			});
	}
}