﻿import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Order, OrderItem } from "../shared/order";

@Component({
	selector: "process",
	templateUrl: "process.component.html",
	styleUrls: ['./process.component.css', '../app/app.component.css']
})
export class ProcessComponent implements OnInit{

	errorMessage: string;

	allOrders: Order[];

	constructor(private data: DataService, private router: Router) {

	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}
		
		this.data.getAllOrders().subscribe(success => {
			if (success) {
				this.allOrders = this.data.allOrders;
			}
		})
	}

	
}