import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlbumDetailsComponent } from './components/album-details/album-details.component';
import { AlbumsByDateComponent } from './components/albums-by-date/albums-by-date.component';
import { AlbumDetailsResolver } from './services/album-details-resolver.service';

const routes: Routes = [
  { path: '', redirectTo: 'dated', pathMatch: 'full' },
  {
    path: 'dated',
    component: AlbumsByDateComponent,
  },
  {
    path: ':artistName/:albumTitle/details',
    component: AlbumDetailsComponent,
    resolve: { album: AlbumDetailsResolver },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class AlbumsRoutingModule {}
