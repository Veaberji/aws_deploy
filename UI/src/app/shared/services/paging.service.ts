import { Injectable } from '@angular/core';
import PagingDetails from '../models/paging-details';

@Injectable({
  providedIn: 'root',
})
export class PagingService {
  private readonly MinFirstPageIndex = 1;

  getPagesArray(pagingDetails: PagingDetails): number[] {
    const firstPage = this.getFirstPage(pagingDetails);
    let size = this.getPagesArraySize(pagingDetails);
    return Array.from({ length: size }, (_, i) => i + firstPage);
  }

  private getFirstPage(pagingDetails: PagingDetails): number {
    const currentPageSize = 1;

    const possiblePagesAmount = this.getPossibleTotalPages(pagingDetails);
    const possibleLinksAfter = Math.trunc(pagingDetails.PagesAmount / 2);
    const calculatedFirstPageIndex = pagingDetails.CurrentPage - possibleLinksAfter;
    const linksAfterCurrent = Math.min(possiblePagesAmount - pagingDetails.CurrentPage, possibleLinksAfter);
    const maxLinksBeforeCurrent = pagingDetails.PagesAmount - linksAfterCurrent - currentPageSize;
    const firstPageIndex = Math.max(calculatedFirstPageIndex, this.MinFirstPageIndex);
    const possibleFirstPageIndex = Math.max(pagingDetails.CurrentPage - maxLinksBeforeCurrent, this.MinFirstPageIndex);

    return Math.min(firstPageIndex, possibleFirstPageIndex);
  }

  private getPossibleTotalPages(pagingDetails: PagingDetails): number {
    return Math.ceil(pagingDetails.TotalItems / pagingDetails.PageSize);
  }

  private getPagesArraySize(pagingDetails: PagingDetails): number {
    const possiblePagesAmount = this.getPossibleTotalPages(pagingDetails);
    return Math.min(possiblePagesAmount, pagingDetails.PagesAmount);
  }
}
