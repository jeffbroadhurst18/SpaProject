import { Http, Response, Headers } from "@angular/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "./product";
import { Order, OrderItem } from "./order";
import 'rxjs/add/operator/map';

@Injectable()
export class DataService {

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
}