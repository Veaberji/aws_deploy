import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

const routes: Routes = [
  { path: '', redirectTo: '/artists', pathMatch: 'full' },
  {
    path: 'artists',
    loadChildren: () => import('./artists/artists.module').then((m) => m.ArtistsModule),
  },
  {
    path: 'albums',
    loadChildren: () => import('./albums/albums.module').then((m) => m.AlbumsModule),
  },
  {
    path: 'not-found',
    component: NotFoundComponent,
  },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
