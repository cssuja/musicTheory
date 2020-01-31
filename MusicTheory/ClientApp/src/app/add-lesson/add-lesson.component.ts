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
  currentQuestion: Question = {} as Question;
  currentOption: QuestionOption = {} as QuestionOption;
  message: string;
  constructor(private addLessonService: AddLessonService,
    private lessonService: LessonsService,
    private questionService: QuestionService) { }

  ngOnInit() {
    this.initialiseCurrentLesson();
    this.getLessons();
  }

  private getLessons() {
    this.lessonService.getLessons().subscribe(lessons => {
      this.lessons = lessons;
    });
  }

  getLesson(id: number) {
    this.questionService.getLesson(id).subscribe(lesson => {
      this.currentLesson = lesson;
    });
  }

  initialiseCurrentLesson() {
    this.currentLesson = {} as Lesson;
    this.currentLesson.questions = [];
    this.initialiseCurrentQuestion();
    this.initialiseCurrentOption();
  }

  initialiseCurrentQuestion() {
    this.currentQuestion = {} as Question;
    this.currentQuestion.options = [];
    this.initialiseCurrentOption();
  }

  initialiseCurrentOption() {
    this.currentOption = {} as QuestionOption;
    this.currentOption.typeId = 1;
  }

  saveLesson() {
    this.addLessonService.saveLesson(this.currentLesson).pipe(
      switchMap(id => this.questionService.getLesson(id)),
      finalize(() => this.initialiseCurrentLesson())
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

  saveQuestion() {
    this.addLessonService.saveQuestion(this.currentQuestion, this.currentLesson.id).pipe(
      finalize(() => this.initialiseCurrentQuestion())
    ).subscribe(
      id => {
        if (this.currentQuestion.id !== id) {
          this.currentQuestion.id = id;
          this.currentLesson.questions = [...this.currentLesson.questions, this.currentQuestion];
        }
        this.getLesson(this.currentLesson.id);
        this.displayMessage('Saved Successfully');
      },
      error => {
        this.displayMessage('Save failed');
      }
    );
  }


  saveOption() {
    this.addLessonService.saveOption(this.currentOption, this.currentQuestion.id).pipe(
      finalize(() => this.initialiseCurrentOption())
    ).subscribe(
      id => {
        if (this.currentOption.id !== id) {
          this.currentOption.id = id;
          this.currentQuestion.options = [...this.currentQuestion.options, this.currentOption];
        }

        this.getLesson(this.currentLesson.id);
        this.displayMessage('Saved Successfully');
      },
      error => {
        this.displayMessage('Save failed');
      }
    );
  }

  deleteLesson() {
    if (confirm('Are you sure to delete this lesson')) {
      this.addLessonService.deleteLesson(this.currentLesson.id).subscribe(
        () => {
          this.initialiseCurrentLesson();
          this.getLessons();
          this.displayMessage('Deleted Successfully');
        },
        error => {
          this.displayMessage('Delete failed');
        }
      );
    }
  }

  deleteQuestion() {
    if (confirm('Are you sure to delete this question')) {
      this.addLessonService.deleteQuestion(this.currentLesson.id, this.currentQuestion.id).subscribe(
        () => {
          this.questionService.getLesson(this.currentLesson.id).subscribe(lesson => {
            this.currentLesson = lesson;
            this.initialiseCurrentQuestion();
          });
          this.displayMessage('Deleted Successfully');
        },
        error => {
          this.displayMessage('Delete failed');
        }
      );
    }
  }

  deleteOption() {
    if (confirm('Are you sure to delete this option')) {
      this.addLessonService.deleteOption(this.currentQuestion.id, this.currentOption.id, this.currentOption.typeId).subscribe(
        () => {
          this.questionService.getLesson(this.currentLesson.id).subscribe(lesson => {
            this.currentLesson = lesson;
            this.initialiseCurrentOption();
            this.currentQuestion.options = this.currentLesson.questions.filter(q => q.id === this.currentQuestion.id)[0].options;
          });
          this.displayMessage('Deleted Successfully');
        },
        error => {
          this.displayMessage('Delete failed');
        }
      );
    }
  }

  onLessonSelect(event) {
    this.getLesson(event.target.value);
  }

  onQuestionSelect(event) {
    this.currentQuestion = this.currentLesson.questions.filter(q => q.id.toString() === event.target.value)[0];
  }

  onOptionSelect(event) {
    this.currentOption = this.currentQuestion.options.filter(o => o.id.toString() === event.target.value)[0];
  }

  private displayMessage(message: string) {
    this.message = message;
    setTimeout(() => {
      this.message = '';
    }, 2000);
  }
}
