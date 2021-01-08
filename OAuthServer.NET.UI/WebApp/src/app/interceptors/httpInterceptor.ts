import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from "../auth/auth.service";

@Injectable()
export class Httpinterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) {}

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const accessToken = this.authService.getToken();
        let updatedRequest: HttpRequest<any>;

        if (accessToken === undefined ||
            accessToken === null ||
            accessToken === '') {
            updatedRequest = request.clone();
            } else {
            updatedRequest = request.clone({
                headers: request.headers.append('Authorization', 'Bearer ' + accessToken)
                    .append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS')
                });
            }

        return next.handle(updatedRequest).pipe(
            tap(
                event => {
                    // log the httpResponse to the browser's console in case of success
                    if (event instanceof HttpResponse) {
                        
                    }
                },
                error => {
                    // log the httpResponse to the browser's console in case of failure
                    if (event instanceof HttpResponse) {
                        
                    }
                }
            )
        )
    }
}