import { Component, OnInit } from '@angular/core';
import { DataService } from "../shared/data.service";

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnInit {

	public isAdmin: boolean;

	constructor(private data: DataService) {
		this.data.loginRequired = false;
	}

	ngOnInit(): void {
		this.isAdmin = false;
	}	

	ngDoCheck() {
		this.isAdmin = this.data.role === 'Admin' ? true : false;
	}
}
