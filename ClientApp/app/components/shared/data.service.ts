﻿import { Http, Response, Headers } from "@angular/http";
import { Injectable, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "./product";
import { User } from "./user";
import { Order, OrderItem } from "./order";
import { Address } from "./address";
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
	public product: Product;

	public orderHistory: Order[] = [];
	public allOrders: Order[] = [];
	public orderItems: OrderItem[] = []; 

	public userName: string = "";
	public user: User;
	public role: string = "";
	public allUsers: string[];
	public address: Address;

	loadProducts(): Observable<Product[]> {
		return this.http.get("/api/products")
			.map((result: Response) =>
				this.products = result.json());
	}

	getProduct(id: number): Observable<Product> {
		return this.http.get("/api/products/" + id)
			.map((result: Response) =>
				this.product = result.json());
	}

	getOrderHistory(user: string): Observable<Order[]> {
		return this.http.get("/api/orders/" + this.userName, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((result: Response) => this.orderHistory = result.json());
	}

	getAllOrders(): Observable<Order[]> {
		return this.http.get("/api/orders/false", {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((result: Response) => this.allOrders = result.json());
	}

	getOrderItems(orderId:number): Observable<OrderItem[]> {
		return this.http.get("/api/orders/orderitems/" + orderId, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((result: Response) => this.orderItems = result.json());
	}

	setStatus(selectedOrder: Order, newStatus: number) {
		selectedOrder.orderStatus = newStatus;
		return this.http.post("/api/orders/setstatus", selectedOrder, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		})
			.map(response => {
				return true;
			});
	}

	calculateTotal(): void {
		this.order.shipping = this.order.overseas ? Math.round(this.order.subtotal * 20)/100 : Math.round(this.order.subtotal * 10)/100;
		this.order.orderTotal = this.order.subtotal + this.order.shipping;
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

	public getRole(user: string) {
		return this.http.get("/api/user/getrole/" + user, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		})
			.map((result: Response) =>
				this.role = result.json());
	}

	public addUser(user: User) {
		return this.http.post("/api/user", user, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((response: Response) => response.json());
	}

	public addAddress(address: Address){
		return this.http.post("/api/user/address", address, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((response: Response) => response.json());
	}

	public updateAddress(userName:string, address: Address) {
		return this.http.put("/api/user/address/" + userName, address, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((response: Response) => response.json());
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

	public getUsers() {
		return this.http.get("/api/user/getusers", {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((result: Response) => this.allUsers = result.json());
	}

	public getUser(username: string) {
		return this.http.get("/api/user/getuser/" + username, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((result: Response) => this.user = result.json());
	}

	public getAddress(username:string) {
		return this.http.get("/api/user/getaddress/" + username, {
			headers: new Headers({ "Authorization": "Bearer " + this.token })
		}).map((result: Response) => this.address = result.json());
	}
}

