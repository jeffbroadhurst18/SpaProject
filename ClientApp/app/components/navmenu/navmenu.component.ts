import { Component } from '@angular/core';
import { DataService } from "../shared/data.service";

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
	constructor(private data: DataService) {
		this.data.loginRequired = false;
	}	

}
