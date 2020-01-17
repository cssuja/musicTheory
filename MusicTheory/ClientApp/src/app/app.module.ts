import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { QuestionComponent } from './question/question.component';
import { QuestionModule } from './question/question.module';
import { LessonsComponent } from './lessons/lessons.component';
import { LessonsModule } from './lessons/lessons.module';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    QuestionModule,
    LessonsModule,
    RouterModule.forRoot([
      { path: '', component: LessonsComponent, pathMatch: 'full' },
      { path: 'lesson/:lessonId', component: QuestionComponent },
      { path: 'lessons', component: LessonsComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
