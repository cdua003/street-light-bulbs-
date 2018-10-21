$(() => {
    ko.applyBindings(new Rsl.ApplicationViewModel(new Rsl.ApiAccess()));
});