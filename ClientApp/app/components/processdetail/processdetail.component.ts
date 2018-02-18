import { Component, OnInit, NgModule, Input, Output, EventEmitter  } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Order, OrderItem } from "../shared/order";
import { Location } from '@angular/common';

@Component({
	selector: "process-detail",
	templateUrl: "processdetail.component.html",
	styleUrls: ['./processdetail.component.css', '../app/app.component.css']
})
export class ProcessDetailComponent implements OnInit{
	@Input() orderId: number;
	@Output() notify: EventEmitter<OrderItem> = new EventEmitter<OrderItem>();
	errorMessage: string;
	orderItems: OrderItem[];

	ngOnInit(): void {
		this.data.getOrderItems(this.orderId).subscribe(success => {
			if (success) {
				this.orderItems = this.data.orderItems;
			}
		})
	}

	constructor(private data: DataService, private router: Router,
				private location: Location) {

	}

	//Comment

	
}