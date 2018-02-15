import { Http, Response, Headers } from "@angular/http";
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
	public product: Product;

	public orderHistory: Order[] = [];
	public allOrders: Order[] = [];

	public userName: string = "";

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