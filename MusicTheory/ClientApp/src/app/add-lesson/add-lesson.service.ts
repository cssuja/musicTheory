import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AddLessonService {

  constructor(private http: HttpClient) { }

  saveLesson(lesson: Lesson) {
    const url = `https://localhost:44366/api/lesson`;
    return this.http.put<number>(url, lesson);
  }

  saveQuestion(question: Question, lessonId: number) {
    const url = `https://localhost:44366/api/lesson/question?lessonId=${lessonId}`;
    return this.http.put<number>(url, question);
  }

  saveOption(option: QuestionOption, questionId: number) {
    const url = `https://localhost:44366/api/lesson/option?questionId=${questionId}`;
    return this.http.put<number>(url, option);
  }
}
