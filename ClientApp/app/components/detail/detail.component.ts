import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router,ActivatedRoute } from "@angular/router";
import { Product } from "../shared/product";
import { Location } from '@angular/common';

@Component({
	selector: "detail",
	templateUrl: "detail.component.html",
	styleUrls: ['./detail.component.css']
})
export class DetailComponent implements OnInit{

	errorMessage: string;
	product: Product;

	constructor(private data: DataService, private router: Router, 
		private activatedRoute: ActivatedRoute, private location: Location) {

	}

	ngOnInit(): void {
		var id = +this.activatedRoute.snapshot.params["id"]; //+converts to number
		this.data.getProduct(id).subscribe(success => {
			if (success) {
				this.product = this.data.product;
			}
		})
	}

	onBack() {
		this.location.back();
	}
	
}