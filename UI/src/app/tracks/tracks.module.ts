import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CommonModule, DatePipe } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { AlbumTrackListComponent } from './components/album-track-list/album-track-list.component';
import { TopTrackListComponent } from './components/track-list/track-list.component';

@NgModule({
  declarations: [TopTrackListComponent, AlbumTrackListComponent],
  providers: [DatePipe],
  imports: [CommonModule, SharedModule, TranslateModule.forChild()],
  exports: [TopTrackListComponent, AlbumTrackListComponent],
})
export class TracksModule {}
