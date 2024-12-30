import { AccountService } from "./../_services/account.service";
import { Component, inject, output } from "@angular/core";
import { FormsModule } from "@angular/forms";

@Component({
	selector: "app-register",
	standalone: true,
	imports: [FormsModule],
	templateUrl: "./register.component.html",
	styleUrl: "./register.component.css",
})
export class RegisterComponent {
	private AccountService: AccountService = inject(AccountService);
	cancelRegister = output<boolean>();
	model: any = {};

	register() {
		this.AccountService.register(this.model).subscribe({
			next: (response: any) => {
				console.log(`Registration successful: ${response}`);
				this.cancel();
			},
			error: (error: any) => {
				console.log(error);
			},
		});
	}

	cancel() {
		this.cancelRegister.emit(false);
	}
}
