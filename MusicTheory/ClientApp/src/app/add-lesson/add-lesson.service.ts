import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AddLessonService {

  constructor(private http: HttpClient) { }

  mergeLesson(lesson: Lesson) {
    console.log('mergeLesson');
    const url = `https://localhost:44366/api/lesson`;
    return this.http.post<Lesson>(url, lesson);
  }

}
