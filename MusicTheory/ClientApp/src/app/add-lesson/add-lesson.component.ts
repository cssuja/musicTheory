import { Component, OnInit } from '@angular/core';
import { AddLessonService } from './add-lesson.service';

@Component({
  selector: 'app-add-lesson',
  templateUrl: './add-lesson.component.html',
  styleUrls: ['./add-lesson.component.css']
})
export class AddLessonComponent implements OnInit {
  lesson: Lesson = {} as Lesson;
  currentQuestion: Question;
  currentOption: QuestionOption;
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
    this.currentQuestion = {} as Question;
    this.currentQuestion.options = [];
    this.currentQuestion.typeId = 1;
  }

  initialiseCurrentTextOption() {
    this.currentOption = {} as QuestionOption;
  }

  addQuestion() {
    this.lesson.questions = [...this.lesson.questions, this.currentQuestion];
    this.initialiseCurrentQuestion();
  }
  addTextOption() {
    console.log(JSON.stringify(this.currentOption));

    this.currentQuestion.options = [...this.currentQuestion.options, this.currentOption];
    this.initialiseCurrentTextOption();
    console.log(JSON.stringify(this.currentOption));
  }

  submit() {
    console.log('submit', this.lesson);
    this.addLessonService.mergeLesson(this.lesson).subscribe();
    this.initialiseLesson();
  }

  onSelect(event) {
    console.log(this.currentOption, event);
  }
}
