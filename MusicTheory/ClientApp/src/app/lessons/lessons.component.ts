import { Component, OnInit } from '@angular/core';
import { LessonsService } from './lessons.service';

@Component({
  selector: 'app-lessons',
  templateUrl: './lessons.component.html',
  styleUrls: ['./lessons.component.css']
})
export class LessonsComponent implements OnInit {
  lessons: Lesson[];
  constructor(private service: LessonsService) { }

  ngOnInit() {
    this.service.getLessons().subscribe(lessons => {
      this.lessons = lessons;
    });
  }

}
