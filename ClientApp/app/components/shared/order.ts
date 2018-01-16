export class Order {
	orderId: number;
	orderDate: Date = new Date();
	orderNumber: string;
	items: Array<OrderItem> = new Array<OrderItem>();
}

export class OrderItem {
	id: number;
	quantity: number;
	unitPrice: number;
	productId: number;
	productCategory: string;
	productSize: string;
	productPrice: number;
	productTitle: string;
	productStockLevel: number;
	productFilePath: string;
}

