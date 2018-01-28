import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";

@Component({
	selector: "history",
	templateUrl: "history.component.html",
	styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit{

	errorMessage: string;

	constructor(private data: DataService, private router: Router) {

	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}
	}

	
}