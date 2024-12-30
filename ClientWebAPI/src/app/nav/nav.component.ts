import { Component } from "@angular/core";
import { AccountService } from "../_services/account.service";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { SharedService } from "../_services/shared.service";

@Component({
	selector: "app-nav",
	standalone: true,
	imports: [BsDropdownModule],
	templateUrl: "./nav.component.html",
	styleUrl: "./nav.component.css",
})
export class NavComponent {
	constructor(
		public AccountService: AccountService,
		public SharedService: SharedService,
	) {}

	logout() {
		this.AccountService.logout();
	}
}
