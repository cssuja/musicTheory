import { Component, OnInit } from '@angular/core';
import { AddLessonService } from './add-lesson.service';

@Component({
  selector: 'app-add-lesson',
  templateUrl: './add-lesson.component.html',
  styleUrls: ['./add-lesson.component.css']
})
export class AddLessonComponent implements OnInit {
  lesson: Lesson = {} as Lesson;
  currentQuestion: QuestionModel;
  currentTextOption: TextQuestionOption;
  currentOptionIsCorrect = false;
  constructor(private addLessonService: AddLessonService) { }

  ngOnInit() {
    this.initialiseLesson();
  }

  initialiseLesson() {
    this.lesson = {} as Lesson;
    this.lesson.questions = [];
    this.initialiseCurrentQuestion();
    this.initialiseCurrentTextOption();
  }

  initialiseCurrentQuestion() {
    this.currentQuestion = {} as QuestionModel;
    this.currentQuestion.textOptions = [];
    this.currentQuestion.typeId = 1;
  }

  initialiseCurrentTextOption() {
    this.currentTextOption = {} as TextQuestionOption;
  }

  addQuestion() {
    this.lesson.questions = [...this.lesson.questions, this.currentQuestion];
    this.initialiseCurrentQuestion();
  }
  addTextOption() {
    this.currentQuestion.textOptions = [...this.currentQuestion.textOptions, this.currentTextOption];
    this.initialiseCurrentTextOption();
  }

  submit() {
    console.log('submit');
    this.addLessonService.mergeLesson(this.lesson).subscribe();
    this.initialiseLesson();
    console.log(this.lesson);
  }

}
