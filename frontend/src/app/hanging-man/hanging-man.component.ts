import { Component, OnInit, Input } from '@angular/core';
import { DataServiceService } from '../data-service.service';

@Component({
  selector: 'app-hanging-man',
  templateUrl: './hanging-man.component.html',
  styleUrls: ['./hanging-man.component.css']
})
export class HangingManComponent implements OnInit {

  @Input() public gameRecord;
  fullImagePath: String;

  constructor(public dataService: DataServiceService) { 
  }

  ngOnInit() {
  }

}
