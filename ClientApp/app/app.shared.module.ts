import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { ShopComponent } from './components/shop/shop.component';
import { ProductListComponent } from './components/shop/product-list.component';
import { CartComponent } from './components/shop/cart.component';
import { DataService } from "./components/shared/data.service";
import { LoginComponent } from './components/login/login.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { HistoryComponent } from './components/history/history.component';
import { DetailComponent } from './components/detail/detail.component';
import { ProcessComponent } from './components/process/process.component';
import { ProcessDetailComponent } from './components/processdetail/processdetail.component';
import { CreateUserComponent } from './components/login/create.user.component';
import { TransformStatusPipe } from "./components/process/process.component.pipe"
import { AddressComponent } from './components/address/address.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
		HomeComponent,
		ShopComponent,
		ProductListComponent,
		CartComponent,
		LoginComponent,
		CheckoutComponent,
		HistoryComponent,
		DetailComponent,
		ProcessComponent,
		ProcessDetailComponent,
		CreateUserComponent,
		AddressComponent,
		TransformStatusPipe,
    ],
    imports: [
        CommonModule,
        HttpModule,
		FormsModule,
		ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
			{ path: 'fetch-data', component: FetchDataComponent },
			{ path: 'the-shop', component: ShopComponent },
			{ path: 'login', component: LoginComponent },
			{ path: 'logout', component: LoginComponent },
			{ path: 'checkout', component: CheckoutComponent },
			{ path: 'history', component: HistoryComponent },
			{ path: 'detail/:id', component: DetailComponent },
			{ path: 'process', component: ProcessComponent },
			{ path: 'processdetail', component: ProcessDetailComponent },
			{ path: 'createuser', component: CreateUserComponent },
			{ path: 'address', component: AddressComponent },
            { path: '**', redirectTo: 'home' }
        ])
	],
	providers: [
		DataService
	]
})
export class AppModuleShared {
}
