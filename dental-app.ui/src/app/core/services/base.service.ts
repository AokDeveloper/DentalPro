import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ListResponseModel, ResponseModel, SingleResponseModel } from '../models/response-model';

// "export" kelimesi EN BAŞTA olmak zorunda
export abstract class BaseService<T> {
  
  protected apiUrl: string;

  constructor(
    protected httpClient: HttpClient, 
    protected endpoint: string
  ) {
    this.apiUrl = environment.apiUrl + endpoint;
  }

  getList(): Observable<ListResponseModel<T>> {
    return this.httpClient.get<ListResponseModel<T>>(this.apiUrl);
  }

  getById(id: number): Observable<SingleResponseModel<T>> {
    return this.httpClient.get<SingleResponseModel<T>>(`${this.apiUrl}/${id}`);
  }

  add(entity: T): Observable<ResponseModel> {
    return this.httpClient.post<ResponseModel>(this.apiUrl, entity);
  }

  update(entity: T): Observable<ResponseModel> {
    return this.httpClient.put<ResponseModel>(this.apiUrl, entity);
  }

  delete(id: number): Observable<ResponseModel> {
    return this.httpClient.delete<ResponseModel>(`${this.apiUrl}/${id}`);
  }
}