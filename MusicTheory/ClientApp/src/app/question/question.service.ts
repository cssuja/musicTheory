import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

    constructor(private http: HttpClient) { }

    getLesson(id: number): Observable<Lesson> {
        const url = `https://localhost:44366/api/question?id=${id}`;
        return this.http.get<Lesson>(url);
    }
}
