import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-breadcrumbs',
  templateUrl: './breadcrumbs.component.html',
  styleUrls: ['./breadcrumbs.component.css'],
  imports: [CommonModule, RouterLink],
})
export class BreadcrumbsComponent {
  @Input() breadcrumbs: { label: string; url?: string }[] = [];
}
