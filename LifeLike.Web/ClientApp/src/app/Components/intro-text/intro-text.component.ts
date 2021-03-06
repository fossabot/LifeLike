import {Component, Input, OnInit} from '@angular/core';
import {Config} from "../../Models/Config";

@Component({
  selector: 'app-intro-text',
  templateUrl: './intro-text.component.html',
  styleUrls: ['./intro-text.component.scss']
})
export class IntroTextComponent implements OnInit {

  constructor() { }
  @Input() config: Config;

  ngOnInit() {
  }

}
