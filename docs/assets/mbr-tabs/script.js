window.addEventListener('hashchange', () => toggleTabs());
window.addEventListener('load', () => toggleTabs());

function toggleTabs(){
    window.removeEventListener(WheelEvent, preventDefault, true);

    let path = window.location.hash.replace("#", "");
    if (path == '')
        return;

    let tabs = document.getElementsByClassName('tab-pane');
    let navlinks = document.getElementsByClassName('nav-link')

    for (let i = 0; i < 3; i++) {
        tabs[i].className = 'tab-pane in';
        navlinks[i].classList.remove('active');
    }

    let itemAnchors = ['AutoHammer', 'FillWand', 'InfinitePaintBucket', 'PaintBrush', 'SpectrePaintBrush', 'MultiWand', 'MirrorWand'];
    let tileAnchors = ['PreHardmodeCraftingStation', 'HardmodeCraftingStation', 'SpecializedCraftingStation', 'ThemedFurnitureCraftingStation', 'MultiCraftingStation'];
    let accessoryAnchors = ['BuildInPeace', 'BuildingWrench', 'UpgradeModules']

    let items = itemAnchors.find(elem => path.toLowerCase() === elem.toLowerCase());
    let tiles = tileAnchors.find(elem => path.toLowerCase() === elem.toLowerCase());
    let accessories = accessoryAnchors.find(elem => path.toLowerCase() === elem.toLowerCase());

    let activeID = null;
    if (items !== undefined) {
        tabs[0].className = 'tab-pane in active';
        navlinks[0].classList.add('active');
        activeID = items;
    }
    else if (tiles !== undefined) {
        tabs[1].className = 'tab-pane in active';
        navlinks[1].classList.add('active');
        activeID = tiles;
    }
    else if (accessories !== undefined) {
        tabs[2].className = 'tab-pane in active';
        navlinks[2].classList.add('active');
        activeID = accessories;
    }

    console.log(activeID)
    if (activeID == null) return;

    document.getElementById(activeID).scrollIntoView({
        block: 'start',
        behavior: 'smooth'
    });
};