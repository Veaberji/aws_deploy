import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { TracksModule } from '../tracks/tracks.module';
import { SharedModule } from '../shared/shared.module';
import { AlbumsRoutingModule } from './albums-routing.module';
import { AlbumDetailsComponent } from './components/album-details/album-details.component';
import { AlbumsByDateComponent } from './components/albums-by-date/albums-by-date.component';
import { TopAlbumsContainerComponent } from './components/albums-container/albums-container.component';

@NgModule({
  declarations: [AlbumDetailsComponent, TopAlbumsContainerComponent, AlbumsByDateComponent],
  imports: [CommonModule, RouterModule, AlbumsRoutingModule, SharedModule, TracksModule, TranslateModule.forChild()],
  exports: [TopAlbumsContainerComponent],
})
export class AlbumsModule {}
