import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { TextInputPipe } from './pipes/text-input.pipe';
import { ButtonComponent } from './components/button/button.component';
import { CardComponent } from './components/card/card.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { FooterComponent } from './components/footer/footer.component';
import { GridComponent } from './components/grid/grid.component';
import { LoadingComponent } from './components/loading/loading.component';
import { MainCardComponent } from './components/main-card/main-card.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { PageSelectorComponent } from './components/page-selector/page-selector.component';
import { PageSizeSelectorComponent } from './components/page-size-selector/page-size-selector.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { ReportFormComponent } from './components/report-form/report-form.component';
import { TabComponent } from './components/tab/tab.component';
import { TableComponent } from './components/table/table.component';

@NgModule({
  declarations: [
    NotFoundComponent,
    NavBarComponent,
    PaginationComponent,
    PageSizeSelectorComponent,
    PageSelectorComponent,
    FooterComponent,
    LoadingComponent,
    CardComponent,
    TableComponent,
    MainCardComponent,
    TabComponent,
    GridComponent,
    DatepickerComponent,
    ButtonComponent,
    ReportFormComponent,
    TextInputPipe,
  ],
  imports: [CommonModule, RouterModule, MaterialModule, FormsModule, ReactiveFormsModule, TranslateModule.forChild()],
  providers: [TextInputPipe],
  exports: [
    NotFoundComponent,
    NavBarComponent,
    PaginationComponent,
    FooterComponent,
    LoadingComponent,
    CardComponent,
    TableComponent,
    MainCardComponent,
    TabComponent,
    GridComponent,
    DatepickerComponent,
    ButtonComponent,
    ReportFormComponent,
    TextInputPipe,
  ],
})
export class SharedModule {}
