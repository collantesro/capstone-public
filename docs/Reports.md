# Reports

The reports are written as MVC Controllers.  Their controllers are in `CCSInventory/Controllers/Reports`.

The HomeController isn't linked to through the side-bar, but it's accessible directly at the url `/reports/home`.  This controller merely displays a view listing the different kinds of reports.  It's not necessary that this be complete, as the sidebar UI has different links for each kind of report.

The ContainersReportController link is named "Inventory" in the sidebar, since the old application used that same name.  If it's confusing, consider renaming it to "Containers".

The reports are similar in functionality to the Index pages that list all the transactions, containers, users, etc.  Those pages provided a search box to filter their contents.  LINQ queries reduced the results to only those relevant to the search term.  Reports query the database (using CCSDbContext) for the date range specified.  Then, in memory, the list of objects are reduced with more LINQ Where statements according to the property values selected by the user.

I'll describe ContainersReportController:

When the user visits `/reports/containers/`, the index view lists all templates from the database.  Currently, this functionality is incomplete, and all reports are saved as templates.  Finish functionality for the Save Template checkbox and only save as template when the user checks that box.

Clicking the New Report button takes the user to `/reports/controllers/newreport`.  There, the user is given the fields necessary to refine their query.  Start/End dates and the Excel option are not saved as part of the template, but are merely runtime options for the report.  I intended for the containers to be filterable based on the Category, Subcategory, or Locations desired by the user.  This functionality wasn't completed.

Mental model for Category/Subcategory filtering:

* Selecting a category implies all its subcategories, present and future.
* Selecting a subcategory unselects its parent category and only filters on those subcategories explicitly chosen.
* Selecting "all" categories implies all categories and their subcategories, present and future.

If the user instead clicks on one of the templates, only the Start/End dates and the Excel options are available to them.  The rest of the form is disabled.  Functionality for editing existing templates has not been written.

The report options are held in a model (`CCSInventory/Models/Reports/ContainerOptions.cs`) that extends from the template (`CCSInventory/Models/Reports/ContainerTemplate.cs`).  When the template is being saved to the database, the report options object is casted to the template object, serialized to JSON, and stored into the template table.  Again, Start/End are not part of the template, they're part of the report options.

Times and dates in the database are stored as UTC.  The user is entering their dates according to their local time (MST/MDT, UTC-7:00/UTC-6:00), but the back-end is likely seeing it as UTC.  When querying for the containers in that date range, the user's input is compared against the UTC dates.  This will result in "missing" containers, unless the DateTime objects are properly converted from local to UTC.

Also, containers modified 24 hours after their created date are instead duplicated, the old container is marked as archived, and the edits are made to the clone.  Selecting the Include Archived option would show all these edited containers.

When exporting to Excel (`CCSInventory/Utilities/ExcelUtil.cs`), the containers are not grouped by any property.  They are reported as-is, since the user viewing the spreadsheet will be able to apply their own structure.

Reports displayed in the browser are grouped by category, and totals are calculated per group.  Export to PDF wasn't implemented, and instead the user is expected to use the print functionality of their browsers to create PDFs.  Windows 10 supports "printing to PDF".

Follow the principles of the UserRoles when extending the functionality of reports.  Readonly users should not be able to create new reports that aren't already pre-existing templates.  Standard users, however, *can* create new reports without a template.  When creating new controllers for the other reports, ensure the proper `[Authorize]` attributes are there for authentication.  Do not allow a logged-out user to visit any of the pages.
