��    +      t              �     �     �     �                .     @     N  #   d     �     �     �     �  #   �          .     K     h     �     �     �     �     �  +     +   .  0   Z  9   �  /   �  /   �     %     D     c  (   �  #   �  "   �  (   �  "     "   ?  *   b     �     �     �  E  �    	  '        B     N  #   [          �     �     �     �     �     �               <     Q     h     �     �  /   �  %   �     �       5   $  &   Z  )   �  (   �     �     �          )     @     ^  "   z     �     �  #   �  '   �  .     %   G  '   m     �   TEXT_COMMAND_USAGE TEXT_EXCEPTION TEXT_SITE_RATING TEXT_SITE_REVIEW TEXT_TESTING_NUMBER_OF_SITES TEXT_TESTING_SITE TEXT_TEST_END TEXT_TEST_RATING_ALLY TEXT_TEST_RATING_INTEGRITY_SECURITY TEXT_TEST_RATING_OVERVIEW TEXT_TEST_RATING_PERFORMANCE TEXT_TEST_RATING_STANDARDS TEXT_TEST_REVIEW_ALLY TEXT_TEST_REVIEW_INTEGRITY_SECURITY TEXT_TEST_REVIEW_OVERVIEW TEXT_TEST_REVIEW_PERFORMANCE TEXT_TEST_REVIEW_RATING_ITEM TEXT_TEST_REVIEW_STANDARDS TEXT_TEST_START TEXT_TEST_START_HEADER TEXT_TEST_VALID_ARGUMENTS TEXT_TEST_VALID_ARGUMENTS_CSS TEXT_TEST_VALID_ARGUMENTS_EMAIL TEXT_TEST_VALID_ARGUMENTS_ENERGY_EFFICIENCY TEXT_TEST_VALID_ARGUMENTS_GOOGLE_LIGHTHOUSE TEXT_TEST_VALID_ARGUMENTS_GOOGLE_LIGHTHOUSE_A11Y TEXT_TEST_VALID_ARGUMENTS_GOOGLE_LIGHTHOUSE_BEST_PRACTICE TEXT_TEST_VALID_ARGUMENTS_GOOGLE_LIGHTHOUSE_PWA TEXT_TEST_VALID_ARGUMENTS_GOOGLE_LIGHTHOUSE_SEO TEXT_TEST_VALID_ARGUMENTS_HTML TEXT_TEST_VALID_ARGUMENTS_HTTP TEXT_TEST_VALID_ARGUMENTS_PA11Y TEXT_TEST_VALID_ARGUMENTS_PAGE_NOT_FOUND TEXT_TEST_VALID_ARGUMENTS_SITESPEED TEXT_TEST_VALID_ARGUMENTS_SOFTWARE TEXT_TEST_VALID_ARGUMENTS_STANDARD_FILES TEXT_TEST_VALID_ARGUMENTS_TRACKING TEXT_TEST_VALID_ARGUMENTS_WEBBKOLL TEXT_TEST_VALID_ARGUMENTS_YELLOW_LAB_TOOLS TEXT_WEBSITE_URL_ADDED TEXT_WEBSITE_URL_DELETED TEXT_WEBSITE_X_OF_Y Project-Id-Version: PACKAGE VERSION
POT-Creation-Date: 2020-05-23 14:06+0200
PO-Revision-Date: YEAR-MO-DA HO:MI+ZONE
Last-Translator: mattias <mattias@webperf.se>
Language-Team: English <team@webperf.se>
MIME-Version: 1.0
Content-Type: text/plain; charset=UTF-8
Content-Transfer-Encoding: 8bit
Generated-By: pygettext.py 1.5
 
	WebPerf Core

	Usage:
default.py -u https://webperf.se

	Options and arguments:
	-h/--help			: Help information on how to use script
	-u/--url <site url>		: website url to test against
	-t/--test <test number>		: run ONE test (use ? to list available tests)
	-r/--review			: show reviews in terminal
	-i/--input <file path>		: input file path (.json/.sqlite)
	-i/--input-skip <number>	: number of items to skip
	-i/--input-take <number>	: number of items to take
	-o/--output <file path>		: output file path (.json/.csv/.sql/.sqlite)
	-A/--addUrl <site url>		: website url (required in compination with -i/--input)
	-D/--deleteUrl <site url>	: website url (required in compination with -i/--input)
	-L/--language <lang code>	: language used for output(en = default/sv) Exception, someone should look at this! ### Rating: ### Review:
 Number of websites being tested {0} # Testing website {0}
 Finished: {0}
 - A11y: {0}
 - Integrity & Security: {0}
 
- Overall: {0}
 - Performance: {0}
 - Standards: {0}
 #### A11y:
{0} #### Integrity & Security:
{0} 
#### Overall:
{0} #### Performance:
{0} {0} ( {1:.2f} rating )
 #### Standards:
{0} Started: {0} ############################################### Valid arguments for option -t/--test: -t 7	: CSS Validation -t 24	: Email (Beta) -t 22	: Energy Efficiency (Website Carbon Calculator) -t 1	: Performance (Google Lighthouse) -t 10	: Accessibility (Google Lighthouse) -t 5	: Best Practice (Google Lighthouse) -t 8	: PWA (Google Lighthouse) -t 4	: SEO (Google Lighthouse) -t 6	: HTML Validation -t 21	: HTTP & Network -t 18	: Accessibility (Pa11y) -t 2	: 404 (Page not Found) -t 15	: Performance (Sitespeed.io) -t 25	: Software (Alpha) -t 9	: Standard files -t 23	: Tracking and Privacy (Beta) -t 20	: Integrity & Security (Webbkoll) -t 17	: Quality on frontend (Yellow Lab Tools) website with url: {0} has been added
 website with url: {0} has been deleted
 Website {0} of {1}.
 