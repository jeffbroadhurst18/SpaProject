import * as _ from "lodash";  //imports everything from this library

export class Order {
	orderId: number;
	orderDate: Date = new Date();
	orderNumber: string;
	orderTotal: number;
	orderStatus: number;
	overseas: boolean = false;
	shipping: number;
	items: Array<OrderItem> = new Array<OrderItem>();
	userName: string;

	get subtotal(): number {
		return _.sum(_.map(this.items, i => i.unitPrice * i.quantity));
	}; //_.map returns an array of calculated fields. _.sum adds these all together. 
}

export class OrderItem {
	id: number;
	quantity: number;
	unitPrice: number;
	productId: number;
	productCategory: string;
	productSize: string;
	productTitle: string;
	productStockLevel: number;
	productFilePath: string;
}

