import { Component, OnInit } from '@angular/core';
import { AddLessonService } from './add-lesson.service';
import { LessonsService } from '../lessons/lessons.service';
import { QuestionService } from '../question/question.service';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-add-lesson',
  templateUrl: './add-lesson.component.html',
  styleUrls: ['./add-lesson.component.css']
})
export class AddLessonComponent implements OnInit {
  lessons: Lesson[] = [];
  currentLesson: Lesson = {} as Lesson;
  currentQuestion: Question;
  currentOption: QuestionOption;
  message: string;
  constructor(private addLessonService: AddLessonService,
    private lessonService: LessonsService,
    private questionService: QuestionService) { }

  ngOnInit() {
    this.initialiseLesson();
    this.getLessons();

  }

  private getLessons() {
    this.lessonService.getLessons().subscribe(lessons => {
      this.lessons = lessons;
    });
  }

  initialiseLesson() {
    this.currentLesson = {} as Lesson;
    this.currentLesson.questions = [];
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

  addTextOption() {
    console.log(JSON.stringify(this.currentOption));

    this.currentQuestion.options = [...this.currentQuestion.options, this.currentOption];
    this.initialiseCurrentTextOption();
    console.log(JSON.stringify(this.currentOption));
  }

  saveLesson() {
    this.addLessonService.mergeLesson(this.currentLesson).pipe(
      switchMap(id => this.questionService.getLesson(id)),
      finalize(() => this.initialiseCurrentQuestion())
    ).subscribe(
      lesson => {
        this.currentLesson = lesson;
        this.getLessons();
        this.displayMessage('Saved successfully');
      },
      error => {
        this.displayMessage('Save failed');
      }
    );
  }

  private displayMessage(message: string) {
    this.message = message;
    setTimeout(() => {
      this.message = '';
    }, 2000);
  }

  addQuestion() {
    this.addLessonService.addQuestion(this.currentQuestion).subscribe(id => {
      console.log(id);
      this.currentQuestion.id = id;
      this.currentLesson.questions = [...this.currentLesson.questions, this.currentQuestion];

      this.save().subscribe(() => {
        this.getLesson(this.currentLesson.id);
      });
    });
  }

  save() {
    console.log('submit', this.currentLesson);
    return this.addLessonService.mergeLesson(this.currentLesson);
  }

  onSelect(event) {
    console.log(this.currentOption, event);
  }

  onLessonSelect(event) {
    this.getLesson(event.target.value);
  }

  getLesson(id: number) {
    this.questionService.getLesson(id).subscribe(lesson => {
      this.currentLesson = lesson;
      console.log(lesson);

    });
  }

  onQuestionSelect(event) {
    this.currentQuestion = this.currentLesson.questions.filter(q => q.id === event.target.value)[0];
  }
}
