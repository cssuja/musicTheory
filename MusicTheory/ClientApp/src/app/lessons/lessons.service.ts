import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class LessonsService {

  constructor(private http: HttpClient) { }

  getLessons(): Observable<Lesson[]> {
    const url = `https://localhost:44366/api/lesson/lessons`;
    return this.http.get<Lesson[]>(url);
  }

  getExistingOptions(): Observable<SelectItem[]> {
    const url = `https://localhost:44366/api/lesson/existingoptions`;
    return this.http.get<SelectItem[]>(url);
  }
}
