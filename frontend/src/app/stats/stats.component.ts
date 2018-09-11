import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css']
})
export class StatsComponent implements OnInit {

  @Input() public gameRecord;
  public incorrectLettersGuessed

  constructor() { }

  ngOnInit() {
    this.incorrectLettersGuessed = this.gameRecord.incorrectLettersGuessed.split('');
  }

}
