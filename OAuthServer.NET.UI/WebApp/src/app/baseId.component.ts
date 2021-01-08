import { MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { BaseComponent } from './base.component';
import { HttpService } from './services/http.service';

export abstract class BaseIdComponent extends BaseComponent {
    id: string;
    
    $routeSubscription: Subscription;
    
    constructor(protected store: Store<IAppState>,
        protected httpService: HttpService,
        protected matDialog: MatDialog,
        protected route: ActivatedRoute) {
        super(store, httpService, matDialog)
     }

    ngOnInit() {
        this.$routeSubscription = this.route.params
            .subscribe(params => {
            this.id = params['id']; // +params converts id to a numbers
        });

        super.ngOnInit();
    }

    ngOnDestroy() {
        super.ngOnDestroy();
        this.$routeSubscription.unsubscribe();
    }
}
