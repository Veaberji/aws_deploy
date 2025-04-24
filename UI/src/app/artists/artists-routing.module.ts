import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TopAlbumsContainerComponent } from '../albums/components/albums-container/albums-container.component';
import { TopTrackListComponent } from '../tracks/components/track-list/track-list.component';
import { ArtistDetailsComponent } from './components/artist-details/artist-details.component';
import { ArtistsContainerComponent } from './components/artists-container/artists-container.component';
import { ArtistsReportComponent } from './components/artists-report/artists-report.component';
import { SimilarArtistsComponent } from './components/similar-artists/similar-artists.component';
import { SupplementRoute } from './supplementRoutes';

const routes: Routes = [
  {
    path: '',
    component: ArtistsContainerComponent,
  },
  {
    path: 'page/:page',
    component: ArtistsContainerComponent,
  },
  {
    path: 'page/:page/pageSize/:pageSize',
    component: ArtistsContainerComponent,
  },
  {
    path: 'details/:name',
    component: ArtistDetailsComponent,
    children: [
      {
        path: '',
        redirectTo: SupplementRoute.TopTracks,
        pathMatch: 'full',
      },
      {
        path: SupplementRoute.TopTracks,
        component: TopTrackListComponent,
      },
      {
        path: SupplementRoute.TopAlbums,
        component: TopAlbumsContainerComponent,
      },
      {
        path: SupplementRoute.SimilarArtists,
        component: SimilarArtistsComponent,
      },
    ],
  },
  {
    path: 'report',
    component: ArtistsReportComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class ArtistsRoutingModule {}
