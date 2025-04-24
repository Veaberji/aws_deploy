import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from '../shared/shared.module';
import { ArtistsRoutingModule } from './artists-routing.module';
import { ArtistDetailsComponent } from './components/artist-details/artist-details.component';
import { ArtistsContainerComponent } from './components/artists-container/artists-container.component';
import { ArtistsReportComponent } from './components/artists-report/artists-report.component';
import { SimilarArtistsComponent } from './components/similar-artists/similar-artists.component';

@NgModule({
  declarations: [ArtistsContainerComponent, ArtistDetailsComponent, SimilarArtistsComponent, ArtistsReportComponent],
  imports: [CommonModule, RouterModule, ArtistsRoutingModule, SharedModule, TranslateModule.forChild()],
})
export class ArtistsModule {}
