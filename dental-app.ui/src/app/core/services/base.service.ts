import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ListResponseModel, ResponseModel, SingleResponseModel } from '../models/response-model';

// 🌟 DİKKAT: <T> yerine <T extends { id: number | string }> yazarak 
// TypeScript'e bu objenin kesinlikle bir ID'si olduğunu garanti ediyoruz.
export abstract class BaseService<T extends { id: number | string }> {
  
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

  // Not: ID'ler GUID (string) veya int (number) olabileceği için 'number | string' yapmak en güvenlisidir.
  getById(id: number | string): Observable<SingleResponseModel<T>> {
    return this.httpClient.get<SingleResponseModel<T>>(`${this.apiUrl}/${id}`);
  }

  add(entity: T): Observable<ResponseModel> {
    return this.httpClient.post<ResponseModel>(this.apiUrl, entity);
  }

  // 🌟 İŞTE DÜZELTİLEN YER: Artık C#'ın beklediği gibi URL'nin sonuna ID'yi otomatik ekliyor
  update(entity: T): Observable<ResponseModel> {
    return this.httpClient.put<ResponseModel>(`${this.apiUrl}/${entity.id}`, entity);
  }

  delete(id: number | string): Observable<ResponseModel> {
    return this.httpClient.delete<ResponseModel>(`${this.apiUrl}/${id}`);
  }
}